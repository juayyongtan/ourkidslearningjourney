using System;
using Android.App;
using Android.Runtime;
namespace PioneerCareSG.AppFolder
{
    [Application]
    public class PioneerCareSGApp : Application
    {
        public PioneerCareSGApp(IntPtr javaRef, JniHandleOwnership transferOwner) : base(javaRef, transferOwner)
        {
            
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
