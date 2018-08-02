using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragDropTrial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/walkthrough-enabling-drag-and-drop-on-a-user-control
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        #region event handlers

        private void panel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                // These Effects values are used in the drag source's
                // GiveFeedback event handler to determine which cursor to display.
                if (e.KeyStates == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }

                Panel _panel = sender as Panel;

                if (_panel != null)
                {
                    Canvas _canvas = GetCanvasFromPanel(_panel);

                    if (_canvas != null)
                    {
                        Circle _circle = e.Data.GetData("Object") as Circle;

                        if (_circle != null)
                        {
                            Point mousePt = e.GetPosition(_canvas);
                            Debug.WriteLine(_panel.Name);
                            Debug.WriteLine(mousePt);

                            PlaceElementInCanvas(_circle, mousePt);

                            Panel _parent = VisualTreeHelper.GetParent(_circle) as Panel;
                            if (_parent != _canvas)
                            {
                                _parent.Children.Remove(_circle);
                                _canvas.Children.Add(_circle);
                            }                            
                        }                        
                    }
                }
            }
        }

        private void panel_Drop(object sender, DragEventArgs e)
        {
            // If an element in the panel has already handled in the drop,
            // the panel should not also handle it.
            if (e.Handled == false)
            {
                Panel _panel = sender as Panel;                
                UIElement _element = e.Data.GetData("Object") as UIElement;

                if (_panel != null && _element != null)
                {
                    // Get the panel that the element currently belongs to,
                    // then remove if from that panel and add it the Children of 
                    // the panel that its been dropped on.
                    Panel _parent = VisualTreeHelper.GetParent(_element) as Panel;

                    if (_parent != null)
                    {
                        if (e.KeyStates == DragDropKeyStates.ControlKey &&
                            e.AllowedEffects.HasFlag(DragDropEffects.Copy))
                        {                            
                            // set the value to return to the DoDragDrop call
                            e.Effects = DragDropEffects.Copy;

                            if (_element is Circle)
                            {
                                Canvas _canvas = GetCanvasFromPanel(panelA);

                                if (_canvas != null)
                                {
                                    Circle _circle = new Circle(_element as Circle);

                                    Point mousePt = e.GetPosition(_canvas);
                                    Debug.WriteLine(mousePt);

                                    PlaceElementInCanvas(_circle, mousePt);

                                    _canvas.Children.Add(_circle);
                                }                                
                            }                            
                        }
                        else if (e.AllowedEffects.HasFlag(DragDropEffects.Move))
                        {                                                      
                            // set the value to return to the DoDragDrop call
                            e.Effects = DragDropEffects.Move;

                            if (_element is Circle)
                            {
                                Canvas _canvas = GetCanvasFromPanel(panelA);

                                if (_canvas != null)
                                {
                                    Point mousePt = e.GetPosition(_canvas);
                                    Debug.WriteLine(mousePt);

                                    PlaceElementInCanvas(_element as FrameworkElement, mousePt);

                                    _parent.Children.Remove(_element);
                                    _canvas.Children.Add(_element);
                                }
                            }                            
                        }
                    }
                }
            }
        }

        #endregion


        private void PlaceElementInCanvas(FrameworkElement element, Point pt)
        {
            Canvas.SetLeft(element, pt.X - element.ActualWidth * 0.5);
            Canvas.SetTop(element, pt.Y - element.ActualHeight * 0.5);
        }

        private Canvas GetCanvasFromPanel(Panel panel)
        {
            Canvas _canvas = null;
            if (panel == panelA)
            {
                _canvas = canvasA;
            }
            else if (panel == panelB)
            {
                _canvas = canvasB;
            }
            return _canvas;
        }
    }
}
