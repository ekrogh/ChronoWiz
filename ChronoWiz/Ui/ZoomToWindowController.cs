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

	private bool _updatePending;
	private double _lastAppliedScale = 1.0;
	private bool _disposed;

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

      _target.AnchorX = _insideScrollView ? 0.0 : 0.5;
		_target.AnchorY = _insideScrollView ? 0.0 : 0.5;

		_page.SizeChanged += OnPageSizeChanged;
		_target.SizeChanged += OnTargetSizeChanged;

		RequestUpdate();
	}

	public void Dispose()
	{
		if (_disposed)
			return;

		_disposed = true;
		_page.SizeChanged -= OnPageSizeChanged;
		_target.SizeChanged -= OnTargetSizeChanged;
	}

	private void OnPageSizeChanged(object? sender, EventArgs e) => RequestUpdate();
	private void OnTargetSizeChanged(object? sender, EventArgs e) => RequestUpdate();

	private void RequestUpdate()
	{
		if (_disposed)
			return;

		if (_updatePending)
			return;

		_updatePending = true;
		try
		{
			_page.Dispatcher.Dispatch(() =>
		{
				if (_disposed)
				{
					_updatePending = false;
					return;
				}

			_updatePending = false;
			UpdateScale();
		});
		}
		catch (ObjectDisposedException)
		{
			_updatePending = false;
		}
	}

	private void UpdateScale()
	{
		if (_disposed)
			return;

		if (_page.Width <= 0 || _page.Height <= 0)
			return;

		// Measure the target at its natural (unscaled) size.
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
      var desiredWidth = _insideScrollView && _target.Width > 0 ? _target.Width : measuredSize.Width;
		var desiredHeight = measuredSize.Height;
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

		if (Math.Abs(newScale - _lastAppliedScale) < 0.01)
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
