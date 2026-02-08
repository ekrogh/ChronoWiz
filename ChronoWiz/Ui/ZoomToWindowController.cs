using Microsoft.Maui.Controls;

namespace ChronoWiz.Ui;

internal sealed class ZoomToWindowController
{
	private readonly ContentPage _page;
	private readonly VisualElement _target;
	private readonly double _minScale;
	private readonly double _maxScale;
	private readonly double _padding;

	private bool _updatePending;
	private double _lastAppliedScale = 1.0;

	public ZoomToWindowController(
		ContentPage page,
		VisualElement target,
		double minScale = 0.6,
		double maxScale = 3.0,
		double padding = 12.0)
	{
		_page = page;
		_target = target;
		_minScale = minScale;
		_maxScale = maxScale;
		_padding = padding;

		_target.AnchorX = 0.5;
		_target.AnchorY = 0.5;

		_page.SizeChanged += OnPageSizeChanged;
		_target.SizeChanged += OnTargetSizeChanged;

		RequestUpdate();
	}

	private void OnPageSizeChanged(object? sender, EventArgs e) => RequestUpdate();
	private void OnTargetSizeChanged(object? sender, EventArgs e) => RequestUpdate();

	private void RequestUpdate()
	{
		if (_updatePending)
			return;

		_updatePending = true;
		_page.Dispatcher.Dispatch(() =>
		{
			_updatePending = false;
			UpdateScale();
		});
	}

	private void UpdateScale()
	{
		if (_page.Width <= 0 || _page.Height <= 0)
			return;

		// Measure the target at its natural (unscaled) size.
		var measuredSize = _target.Measure(double.PositiveInfinity, double.PositiveInfinity);
		var desiredWidth = measuredSize.Width;
		var desiredHeight = measuredSize.Height;
		if (desiredWidth <= 0 || desiredHeight <= 0)
			return;

		var availableWidth = Math.Max(0, _page.Width - _padding * 2);
		var availableHeight = Math.Max(0, _page.Height - _padding * 2);
		if (availableWidth <= 0 || availableHeight <= 0)
			return;

		var scaleX = availableWidth / desiredWidth;
		var scaleY = availableHeight / desiredHeight;
		var newScale = Math.Min(scaleX, scaleY);
		newScale = Math.Clamp(newScale, _minScale, _maxScale);

		if (Math.Abs(newScale - _lastAppliedScale) < 0.01)
			return;

		_lastAppliedScale = newScale;
		_target.Scale = newScale;
	}
}
