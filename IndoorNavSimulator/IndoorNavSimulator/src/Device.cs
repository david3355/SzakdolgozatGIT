﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

namespace IndoorNavSimulator
{
    abstract class Device
    {
        public Device(Canvas Background)
        {
            background = Background;
            BaseInit();
        }

        static Device()
        {
            red = new SolidColorBrush(Colors.Red);
            green = new SolidColorBrush(Colors.Green);
        }

        protected Ellipse deviceDisplay;
        protected Canvas background;
        protected Point origo;
        protected double radius;
        protected const double commonRadius = 12;

        protected static SolidColorBrush red, green;

        protected abstract void Init();

        public double Radius
        {
            get { return radius; }
        }

        private void BaseInit()
        {
            origo = new Point(100, 100);
            deviceDisplay = new Ellipse();
            Init();
            Display();
        }

        public void ApplyRadius()
        {
            deviceDisplay.Width = radius * 2;
            deviceDisplay.Height = radius * 2;
        }

        public void Display()
        {
            if (!background.Children.Contains(deviceDisplay)) background.Children.Add(deviceDisplay);
        }

        public void Hide()
        {
            if (this.deviceDisplay.Visibility != Visibility.Hidden)
                this.deviceDisplay.Visibility = Visibility.Hidden;
        }

        public void Visible()
        {
            if (this.deviceDisplay.Visibility != Visibility.Visible)
                this.deviceDisplay.Visibility = Visibility.Visible;
        }

        public void MoveDevice(Point Center)
        {
            //ide még annyit, hogy vár egy másik paramétert, amivel be lehet állítani a méretet, hogy ha 1 tag van csak, akkor az egész radius a jelölt hely
            origo = Center;
            Canvas.SetLeft(deviceDisplay, origo.X - radius);
            Canvas.SetTop(deviceDisplay, origo.Y - radius);
        }

        public void SetRadius(int Radius)
        {
            radius = Radius;
        }


    }

    class RealDevice : Device
    {
        public RealDevice(Canvas Background)
            : base(Background)
        {

        }

        protected override void Init()
        {
            radius = commonRadius - 2;
            ApplyRadius();
            deviceDisplay.Fill = green;
        }

    }

    class SimulatedDevice : Device
    {
        public SimulatedDevice(Canvas Background)
            : base(Background)
        {
            precision = Precision.NoTag;
        }

        private Precision precision;

        public Precision Precision
        {
            get { return precision; }
        }

        protected override void Init()
        {
            deviceDisplay.Visibility = Visibility.Hidden;
            radius = commonRadius;
            ApplyRadius();
            deviceDisplay.Stroke = red;
            deviceDisplay.StrokeThickness = 2;
            deviceDisplay.Fill = red;
            deviceDisplay.Fill.Opacity = 0.3;
        }

        public void SetSimulationDisplay(Precision Precision, Point Origo, double Radius = -1)
        {
            if (this.precision != Precision) precision = Precision;
            this.origo = Origo;
            if (Radius < commonRadius) Radius = commonRadius;

            switch (Precision)
            {
                case Precision.NoTag:
                    Hide();
                    break;
                case Precision.OneTag:
                    Visible();
                    this.radius = Radius;
                    break;
                case Precision.TwoTag:
                    Visible();
                    this.radius = Radius;
                    break;
                case Precision.ThreeOrMoreTag:
                    Visible();
                    this.radius = commonRadius;
                    break;
            }

            ApplyRadius();
        }

    }

}
