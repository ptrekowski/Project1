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

            // disable for debugging
            input.KeyboardFilterMode = KeyboardFilterMode.All;
        }

        public void startMoveForward()
        {
            //input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.SendKey(Interceptor.Keys.A, KeyState.Down);
            //input.KeyboardFilterMode = KeyboardFilterMode.None;
        }

        public void stopMoveForward()
        {
            //input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.SendKey(Interceptor.Keys.A, KeyState.Up);
            //input.KeyboardFilterMode = KeyboardFilterMode.None;
        }

        public void turnLeft()
        {
            input.SendKey(Interceptor.Keys.D, KeyState.Down);
            System.Threading.Thread.Sleep(50);
            input.SendKey(Interceptor.Keys.D, KeyState.Up);
        }

        public void turnRight()
        {
            input.SendKey(Interceptor.Keys.F, KeyState.Down);
            System.Threading.Thread.Sleep(50);
            input.SendKey(Interceptor.Keys.F, KeyState.Up);
        }

        public void mouseRightDown()
        {
            input.SendMouseEvent(MouseState.RightDown);
        }

        public void mouseRightUp()
        {
            input.SendMouseEvent(MouseState.RightUp);
        }

        public void tapMouseRight()
        {
            input.MoveMouseBy(10, 0, true);
        }

        public void tapMouseLeft()
        {
            input.MoveMouseBy(-10, 0, true);
        }
        public void attack1()
        {
            input.SendKey(Keys.One);
        }

        public void attack2()
        {
            input.SendKey(Keys.Two);
        }

        public void findTarget()
        {
            input.SendKey(Keys.Tab);
        }

        public void pullMob()
        {
            input.SendKey(Keys.Four);
        }

        public void faceMob()
        {
            input.SendKey(Keys.B);
        }


    }
}
