using Microsoft.Maui.Controls;

namespace ChronoWiz.Ui;

internal sealed class ZoomToWindowController : IDisposable
{
	private readonly ContentPage _page;
	private readonly VisualElement _target;
	private readonly double _minScale;
	private readonly double _maxScale;
	private readonly double _padding;
	private readonly bool _insideScrollView;
	private readonly IDispatcherTimer _debounceTimer;

	private bool _updatePending;
	private double _lastAppliedScale = 1.0;
	private bool _disposed;
	private int _layoutChangeDepth;

	public ZoomToWindowController(
		ContentPage page,
		VisualElement target,
      double minScale = 1.0,
		double maxScale = 3.0,
		double padding = 12.0)
	{
		_page = page;
		_target = target;
		_minScale = minScale;
		_maxScale = maxScale;
		_padding = padding;
		_insideScrollView = IsInsideScrollView(target);

		_debounceTimer = _page.Dispatcher.CreateTimer();
		_debounceTimer.IsRepeating = false;
		_debounceTimer.Interval = TimeSpan.FromMilliseconds(75);
		_debounceTimer.Tick += OnDebounceTick;

      _target.AnchorX = _insideScrollView ? 0.0 : 0.5;
		_target.AnchorY = _insideScrollView ? 0.0 : 0.5;

		_page.SizeChanged += OnPageSizeChanged;

		RequestUpdate();
	}

	public void Dispose()
	{
		if (_disposed)
			return;

		_disposed = true;
		_debounceTimer.Stop();
		_debounceTimer.Tick -= OnDebounceTick;
		_page.SizeChanged -= OnPageSizeChanged;
	}

	public void BeginLayoutChange()
	{
		if (_disposed)
			return;

		_layoutChangeDepth++;
		if (_debounceTimer.IsRunning)
			_debounceTimer.Stop();
		_updatePending = false;
	}

	public void EndLayoutChange()
	{
		if (_disposed)
			return;

		if (_layoutChangeDepth > 0)
			_layoutChangeDepth--;

		if (_layoutChangeDepth == 0)
			RequestUpdate();
	}

	private void OnPageSizeChanged(object? sender, EventArgs e) => RequestUpdate();

	private void RequestUpdate()
	{
		if (_disposed)
			return;

		if (_layoutChangeDepth > 0)
			return;

		if (_updatePending)
			return;

		_updatePending = true;
		try
		{
			if (_debounceTimer.IsRunning)
				_debounceTimer.Stop();
			_debounceTimer.Start();
		}
		catch (ObjectDisposedException)
		{
			_updatePending = false;
		}
	}

	private void OnDebounceTick(object? sender, EventArgs e)
	{
		if (_disposed)
			return;

		_updatePending = false;
		UpdateScale();
	}

	private void UpdateScale()
	{
		if (_disposed)
			return;

		if (_page.Width <= 0 || _page.Height <= 0)
			return;

		// Measure the target at its natural size.
		Size measuredSize;
		try
		{
			measuredSize = _target.Measure(double.PositiveInfinity, double.PositiveInfinity);
		}
		catch (ObjectDisposedException)
		{
			Dispose();
			return;
		}
		// Prefer arranged size (Width/Height), which is stable after orientation layout is applied.
		var arrangedWidth = _target.Width > 0 ? _target.Width : 0;
		var arrangedHeight = _target.Height > 0 ? _target.Height : 0;

		var desiredWidth = arrangedWidth > 0 ? arrangedWidth : measuredSize.Width;
		var desiredHeight = arrangedHeight > 0 ? arrangedHeight : measuredSize.Height;
		if (desiredWidth <= 0 || desiredHeight <= 0)
			return;

		var availableWidth = Math.Max(0, _page.Width - _padding * 2);
		var availableHeight = Math.Max(0, _page.Height - _padding * 2);
		if (availableWidth <= 0 || availableHeight <= 0)
			return;

		var scaleX = availableWidth / desiredWidth;
		var scaleY = availableHeight / desiredHeight;
		var newScale = _insideScrollView
			? Math.Max(1.0, scaleX)
			: Math.Min(scaleX, scaleY);
		newScale = Math.Clamp(newScale, _minScale, _maxScale);
		newScale = Math.Round(newScale, 2, MidpointRounding.AwayFromZero);

		if (Math.Abs(newScale - _lastAppliedScale) < 0.02)
			return;

		_lastAppliedScale = newScale;
		_target.Scale = newScale;
	}

	private static bool IsInsideScrollView(Element element)
	{
		for (Element? current = element.Parent; current is not null; current = current.Parent)
		{
			if (current is ScrollView)
				return true;
		}

		return false;
	}
}
