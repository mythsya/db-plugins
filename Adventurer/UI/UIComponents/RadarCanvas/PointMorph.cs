﻿using System;
using System.Windows;
using Zeta.Common;
using Logger = Adventurer.Util.Logger;

namespace Adventurer.UI.UIComponents.RadarCanvas
{
    /// <summary>
    /// PointMorph handles the translation of a Vector3 world space position into Canvas space.
    /// </summary>
    public class PointMorph
    {
        public PointMorph() { }

        public PointMorph(CanvasData canvasData)
        {
            CanvasData = canvasData;
        }

        /// <summary>
        /// PointMorph handles the translation of a Vector3 world space position into Canvas space.
        /// </summary>
        public PointMorph(Vector3 vectorPosition, CanvasData canvasData)
        {
            CanvasData = canvasData;
            Update(vectorPosition);
        }

        /// <summary>
        /// Information about the canvas
        /// </summary>
        public CanvasData CanvasData { get; set; }

        /// <summary>
        /// Point in GridSquare (Yards) Space before translations
        /// </summary>
        public Point RawGridPoint { get; set; }

        /// <summary>
        /// Point before any translations
        /// </summary>
        public Point RawPoint { get; set; }

        /// <summary>
        /// Flipped and Rotated point
        /// </summary>
        public Point Point { get; set; }

        /// <summary>
        /// Point coods based on Grid Scale
        /// </summary>
        public Point GridPoint { get; set; }

        /// <summary>
        /// If the point is located outside of the canvas bounds
        /// </summary>
        public bool IsBeyondCanvas { get; set; }

        /// <summary>
        /// If the point is Zero
        /// </summary>
        public bool IsZero
        {
            get { return Point.X == 0 || Point.Y == 0; }
        }

        /// <summary>
        /// Game world distance from this point to the center actor on X-Axis
        /// </summary>
        public float RawWorldDistanceX { get; set; }

        /// <summary>
        /// Game world distance from this point to the center actor on Y-Axis
        /// </summary>
        public float RawWorldDistanceY { get; set; }

        /// <summary>
        /// Canvas (pixels) distance from this point to the center actor on Y-Axis
        /// </summary>
        public float RawDrawDistanceX { get; set; }

        /// <summary>
        /// Canvas (pixels) distance from this point to the center actor on Y-Axis
        /// </summary>
        public float RawDrawDistanceY { get; set; }

        /// <summary>
        /// Absolute canvas X-Axis coodinate for this actor (in pixels)
        /// </summary>
        public double RawDrawPositionX { get; set; }

        /// <summary>
        /// Absolute canvas Y-Axis coodinate for this actor (in pixels)
        /// </summary>
        public double RawDrawPositionY { get; set; }

        /// <summary>
        /// Absolute game world vector
        /// </summary>
        public Vector3 WorldVector { get; set; }

        /// <summary>
        /// Calculates Canvas position with a given game world position
        /// </summary>
        public void Update(Vector3 position)
        {
            try
            {
                WorldVector = position;

                var centerActorPosition = CanvasData.CenterVector;

                // Distance from Actor to Player
                RawWorldDistanceX = centerActorPosition.X - position.X;
                RawWorldDistanceY = centerActorPosition.Y - position.Y;

                // We want 1 yard of game distance to = Gridsize
                RawDrawDistanceX = RawWorldDistanceX * (float)CanvasData.GridSquareSize.Width;
                RawDrawDistanceY = RawWorldDistanceY * (float)CanvasData.GridSquareSize.Height;

                // Distance on canvas from center to actor
                RawDrawPositionX = (CanvasData.Center.X + RawDrawDistanceX);
                RawDrawPositionY = (CanvasData.Center.Y + RawDrawDistanceY);

                // Points in Canvas and Grid Scale
                RawPoint = new Point(RawDrawPositionX, RawDrawPositionY);
                RawGridPoint = new Point(RawDrawPositionX / CanvasData.GridSquareSize.Width, RawDrawPositionY / CanvasData.GridSquareSize.Height);

                // Switched to manual calculations because WPF transforms are very slow 
                // (0.0015ms+ each versus 0.0000ms for raw math).
                Point = RawPoint.Rotate(CanvasData.Center, CanvasData.GobalRotationAngle);
                Point = Point.FlipX(CanvasData.Center);               

                GridPoint = new Point((int)(Point.X / CanvasData.GridSquareSize.Width), (int)(Point.Y / CanvasData.GridSquareSize.Height));
                IsBeyondCanvas = Point.X < 0 || Point.X > CanvasData.CanvasSize.Width || Point.Y < 0 || Point.Y > CanvasData.CanvasSize.Height;
            }
            catch (Exception ex)
            {
                Logger.Debug("Exception in RadarUI.PointMorph.Update(). {0} {1}", ex.Message, ex.InnerException);
            }
        }


    }
}
