using System;
using System.Diagnostics;
// using System.Windows;
using System.Runtime.InteropServices;

namespace WinterEngine
{
     class FocusStateMGMT
     {
          /// Return Type: BOOL->int
          ///fBlockIt: BOOL->int
          [DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
          [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
          public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

          [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
          private static extern IntPtr GetForegroundWindow();

          [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
          private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);


          public static bool ApplicationIsActivated()
          {
               var activatedHandle = GetForegroundWindow();
               if (activatedHandle == IntPtr.Zero)
               {
                    return false;       // No window is currently activated
               }

               var procId = Process.GetCurrentProcess().Id;
               int activeProcId;
               GetWindowThreadProcessId(activatedHandle, out activeProcId);

               return activeProcId == procId;
          }

          public static bool CheckHasFocus()
          {
               if (ApplicationIsActivated())
               {

                    // NativeMethods.BlockInput(true);
                    // BlockInput DOESN'T affect GetKeyboardState so ...
                    return true;
               }
               else
                    // NativeMethods.BlockInput(false);
                    return false;
          }
     }
}
