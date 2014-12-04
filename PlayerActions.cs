using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interceptor;

namespace Penguin2
{
    class PlayerActions
    {
        // Interceptor
        Input input = new Input();

        public PlayerActions()
        {
            input.Load();
            //input.KeyboardFilterMode = KeyboardFilterMode.All;
        }

        public void startMoveForward()
        {
            input.SendKey(Interceptor.Keys.A, KeyState.Down);
        }

        public void stopMoveForward()
        {
            input.SendKey(Interceptor.Keys.A, KeyState.Up);
        }

        public void turnLeft()
        {
            input.SendKey(Interceptor.Keys.D);
        }

        public void turnRight()
        {
            input.SendKey(Interceptor.Keys.F);
        }

        public void attack1()
        {
            input.SendText("hi");
            input.SendKey(Keys.Two);
        }
    }
}
