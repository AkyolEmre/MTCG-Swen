﻿using System;

namespace Monster_Trading_Cards_Game
{

    /// <summary>Handlers that handle HTTP requests implement this interface.</summary>
    public interface IHandler
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Tries to handle a HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns TRUE if the request was handled by this instance,
        ///          otherwise returns FALSE.</returns>
        public bool Handle(HttpSvrEventArgs e);
    }
}