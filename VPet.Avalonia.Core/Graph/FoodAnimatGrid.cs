using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace VPet.Avalonia.Core.Graph;

public class FoodAnimatGrid : Panel
{
    public Image Front;
    public Image Food;
    public Image Back;

    public FoodAnimatGrid()
    {
        Width = 500;
        Height = 500;
        VerticalAlignment = VerticalAlignment.Top;
        HorizontalAlignment = HorizontalAlignment.Left;
        Front = new Image();
        Back = new Image();
        Food = new Image
        {
            RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            IsVisible = false
        };
        Children.Add(Back);
        Children.Add(Food);
        Children.Add(Front);
    }
}