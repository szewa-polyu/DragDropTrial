using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragDropTrial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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

                Panel _panel = (Panel)sender;
                Canvas _canvas;
                if (_panel == panelA)
                {
                    _canvas = canvasA;
                }
                else
                {
                    _canvas = canvasB;
                }

                Circle _circle = (Circle)e.Data.GetData("Object");

                Point mousePt = e.GetPosition(_canvas);

                Debug.WriteLine(mousePt);

                Canvas.SetLeft(_circle, mousePt.X - (_circle as Circle).ActualWidth * 0.5);
                Canvas.SetTop(_circle, mousePt.Y - (_circle as Circle).ActualHeight * 0.5);
            }
        }

        private void panel_Drop(object sender, DragEventArgs e)
        {
            // If an element in the panel has already handled in the drop,
            // the panel should not also handle it.
            if (e.Handled == false)
            {
                Panel _panel = (Panel)sender;
                Canvas _canvas;
                if (_panel == panelA)
                {
                    _canvas = canvasA;
                }
                else
                {
                    _canvas = canvasB;
                }
                UIElement _element = (UIElement)e.Data.GetData("Object");

                if (_panel != null && _element != null)
                {
                    // Get the panel that the element currently belongs to,
                    // then remove if from that panel and add it the Children of 
                    // the panel that its been dropped on.
                    Panel _parent = (Panel)VisualTreeHelper.GetParent(_element);

                    if (_parent != null)
                    {
                        if (e.KeyStates == DragDropKeyStates.ControlKey &&
                            e.AllowedEffects.HasFlag(DragDropEffects.Copy))
                        {
                            Circle _circle = new Circle((Circle)_element);

                            Point mousePt = e.GetPosition(_canvas);

                            Debug.WriteLine(mousePt);

                            Canvas.SetLeft(_circle, mousePt.X - (_circle as Circle).ActualWidth * 0.5);
                            Canvas.SetTop(_circle, mousePt.Y - (_circle as Circle).ActualHeight * 0.5);
                            
                            _canvas.Children.Add(_circle);

                            // set the value to return to the DoDragDrop call
                            e.Effects = DragDropEffects.Copy;
                        }
                        else if (e.AllowedEffects.HasFlag(DragDropEffects.Move))
                        {
                            Point mousePt = e.GetPosition(_canvas);

                            Debug.WriteLine(mousePt);

                            Canvas.SetLeft(_element, mousePt.X - (_element as Circle).ActualWidth * 0.5);
                            Canvas.SetTop(_element, mousePt.Y - (_element as Circle).ActualHeight * 0.5);

                            _parent.Children.Remove(_element);
                            _canvas.Children.Add(_element);                            

                            // set the value to return to the DoDragDrop call
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
