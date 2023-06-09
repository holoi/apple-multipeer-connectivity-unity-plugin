// SPDX-FileCopyrightText: Copyright 2023 Holo Interactive <dev@holoi.com>
// SPDX-FileCopyrightText: Copyright 2020 Unity Technologies ApS
// SPDX-FileContributor: Yuchen Zhang <yuchen@holoi.com>
// SPDX-License-Identifier: MIT AND LicenseRef-Unity-Companion-License

using System;
using System.Runtime.InteropServices;

namespace HoloInteractive.iOS.MultipeerConnectivity
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSError : IDisposable, IEquatable<NSError>
    {
        IntPtr m_Ptr;

        public bool Valid => m_Ptr != IntPtr.Zero;

        public NSErrorException ToException()
        {
            return new NSErrorException(Code, Description);
        }

        public long Code
        {
            get
            {
                if (!Valid)
                    throw new InvalidOperationException($"The {typeof(NSError).Name} is not valid.");

                return GetCode(this);
            }
        }

        public string Description
        {
            get
            {
                if (!Valid)
                    throw new InvalidOperationException($"The {typeof(NSError).Name} is not valid.");

                using (var description = GetLocalizedDescription(this))
                {
                    return description.ToString();
                }
            }
        }

        public void Dispose() => NativeApi.CFRelease(ref m_Ptr);
        public override int GetHashCode() => m_Ptr.GetHashCode();
        public override bool Equals(object obj) => (obj is NSError) && Equals((NSError)obj);
        public bool Equals(NSError other) => m_Ptr == other.m_Ptr;
        public static bool operator ==(NSError lhs, NSError rhs) => lhs.Equals(rhs);
        public static bool operator !=(NSError lhs, NSError rhs) => !lhs.Equals(rhs);

        [DllImport("__Internal", EntryPoint = "HoloInteractiveMC_NSError_code")]
        static extern long GetCode(NSError error);

        [DllImport("__Internal", EntryPoint = "HoloInteractiveMC_NSError_localizedDescription")]
        static extern NSString GetLocalizedDescription(NSError error);
    }
}