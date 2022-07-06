using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the snake 
    /// collides with the food, or the snake collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool isGameOver = false; 
        private bool player1Won = false;
        private bool player2Won = false;

        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)

        {
            if (isGameOver == false)
            {
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
            }
        }

        /// <summary>
        /// Sets the game over flag if the snake collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Snake snake = (Snake)cast.GetFirstActor("snake");
            Snake snake2 = (Snake)cast.GetFirstActor("snake2");
            Actor head = snake.GetHead();
            Actor head2 = snake2.GetHead();
            List<Actor> body = snake.GetBody();
            List<Actor> body2 = snake2.GetBody();

            snake.GrowTail(1);
            snake2.GrowTail(1);

            foreach (Actor segment in body)
            {
                if (head2.GetPosition().Equals(segment.GetPosition()))
                {
                        isGameOver = true;
                        player1Won = true;
                }
            }

            foreach (Actor segment in body2)
            {
                if (head.GetPosition().Equals(segment.GetPosition()))
                {
                        isGameOver = true;
                        player2Won = true;
                }
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (isGameOver == true)
            {
                Snake snake = (Snake)cast.GetFirstActor("snake");
                Snake snake2 = (Snake)cast.GetFirstActor("snake2");
                List<Actor> segments = snake.GetSegments();
                List<Actor> segments2 = snake2.GetSegments();

                // create a "game over" message
                int x = Constants.MAX_X / 2;
                int y = Constants.MAX_Y / 2;
                Point position = new Point(x, y);

                Actor message = new Actor();
                
                message.SetPosition(position);
                cast.AddActor("messages", message);

                if (player1Won)
                {
                    foreach (Actor segment in segments2)
                    {
                        segment.SetColor(Constants.WHITE);
                    }
                    message.SetText("Game Over! Player 1 won (RED)");
                }
                

                if (player2Won)
                {
                    foreach (Actor segment in segments)
                    {
                        segment.SetColor(Constants.WHITE);
                    }
                    message.SetText("Game Over! Player 2 won (GREEN)");
                }
            }
        }

    }
}