#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace CataclysmMod.Common.UserInterface
{
    /// <summary>
    ///     Simple class for handling user interfaces in Terraria. Requires each state to be that of a unique type.
    /// </summary>
    public class UserInterfaceHandler
    {
        public delegate UIState UIStateFactory();

        public delegate T UIStateFactory<out T>() where T : UIState;

        public readonly Terraria.UI.UserInterface Interface;
        public readonly Dictionary<Type, UIState> RegisteredStates = new Dictionary<Type, UIState>();

        private GameTime GameTimeCache;

        public UserInterfaceHandler()
        {
            Interface = new Terraria.UI.UserInterface();
        }

        public UIState RegisterState(UIState state)
        {
            Type type = state.GetType();

            if (RegisteredStates.ContainsKey(type))
                throw new Exception($"UIState \"{type.FullName}\" has already been registered!");

            RegisteredStates[type] = state;

            return state;
        }

        public T RegisterState<T>() where T : UIState, new() => (T) RegisterState(new T());

        public UIState RegisterState(UIStateFactory factory) => RegisterState(factory());

        public T RegisterState<T>(UIStateFactory<T> factory) where T : UIState => (T) RegisterState(factory());

        public void ToggleState(Type stateType)
        {
            if (!RegisteredStates.ContainsKey(stateType))
                throw new Exception($"UIState \"{stateType.FullName}\" has not been registered!");

            Interface.SetState(Interface.CurrentState == RegisteredStates[stateType]
                ? null
                : RegisteredStates[stateType]);
        }

        public void ToggleState<T>() where T : UIState => ToggleState(typeof(T));

        public void ToggleState(UIState state) => ToggleState(state.GetType());

        public bool IsVisible(Type stateType)
        {
            if (!RegisteredStates.ContainsKey(stateType))
                throw new Exception($"UIState \"{stateType.FullName}\" has not been registered!");

            return Interface.CurrentState == RegisteredStates[stateType];
        }

        public bool IsVisible<T>() where T : UIState => IsVisible(typeof(T));

        public bool IsVisible(UIState state) => IsVisible(state.GetType());

        public UIState GetState(Type stateType)
        {
            if (!RegisteredStates.ContainsKey(stateType))
                throw new Exception($"UIState \"{stateType.FullName}\" has not been registered!");

            return RegisteredStates[stateType];
        }

        public T GetState<T>() where T : UIState => (T) GetState(typeof(T));

        public void UpdateStates(GameTime gameTime)
        {
            GameTimeCache = gameTime;
            Interface.CurrentState?.Update(gameTime);
        }

        public void DrawStates(SpriteBatch spriteBatch)
        {
            if (GameTimeCache != null)
                Interface.Draw(spriteBatch, GameTimeCache);
        }
    }
}