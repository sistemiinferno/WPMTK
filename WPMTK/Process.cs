﻿using System;

namespace WPMTK
{
    /// <summary>
    /// A Process object defines the attributes of a running windowed application on your computer.
    /// NOTE: The Process class locates processes on your computer based on the window title you specify in the constructor.
    /// </summary>
    public class Process : IDisposable
    {
        public static Exception ProcessNotFoundException = new Exception(
            "Could not find the process specified. " +
            "Please reference the process by it's window title exactly as it appears. " +
            "E.x. \"Mount&Blade\".");
        public VAMemory memory;
        private IntPtr hWnd;
        private string windowTitle;

        public string WindowTitle { get => windowTitle; private set => windowTitle = value; }
        private bool disposed = false;

        public Process(string window_title)
        {
            if (!SethWnd(WindowTitle)) // true if succeeded
            {
                throw ProcessNotFoundException;
            }
            memory = new VAMemory(WindowTitle);
            WindowTitle = window_title;
        }

        #region Getters
        /// <summary>
        /// Uses NativeMethods.GetWindowRect() to retrieve the RECT of the process's main window.
        /// </summary>
        /// <returns>NativeMethods.RECT</returns>
        public NativeMethods.RECT GetWindowRect()
        {
            NativeMethods.GetWindowRect(hWnd, out NativeMethods.RECT rect);
            return rect;
        }
        #endregion

        #region hWnd & VAMemory
        /// <summary>
        /// Before any memory can be used, or overlays can be drawn, the process must be attached. Should be within a "try" block.
        /// </summary>
        /// <exception cref="ProcessNotFoundException"></exception>
        public void Attach()
        {
            
        }
        
        private bool SethWnd(string title)
        {
            try
            {
                hWnd = NativeMethods.FindWindow(null, title);
                if (hWnd == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get the HWND that belongs to this process.
        /// </summary>
        /// <returns>IntPtr (hWnd)</returns>
        public IntPtr GethWnd()
        {
            return hWnd;
        }

        /// <summary>
        /// Changes this object to attach to an entirely different process.
        /// </summary>
        /// <param name="window_title">Name of process to attach to.</param>
        /// <returns>True if no errors, false if failed to locate new process: returns to same process.</returns>
        /*public void ChangeProcess(string window_title)
        {
            if (SethWnd(window_title)) // if false, failed
            {
                memory = new VAMemory(window_title);
                windowTitle = window_title;
            }
            else
            {
                throw ProcessNotFoundException;
            }
        }*/
        #endregion

        #region Disposal
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected unsafe virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // managed resources to dispose
                if (disposing)
                {
                    // none
                }
                try {
                    NativeMethods.CloseHandle(hWnd);
                } catch (Exception ex) {
                    Environment.FailFast(ex.Message);
                }
                hWnd = IntPtr.Zero;
                disposed = true;
            }
        }

        ~Process()
        {
            Dispose(false);
        }
        #endregion
    }
}