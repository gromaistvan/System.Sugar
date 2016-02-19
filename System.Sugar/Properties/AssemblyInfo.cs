using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct( "System.Sugar" )]
[assembly: AssemblyTitle( "System.Sugar" )]
[assembly: AssemblyDescription( "Useful .NET extensions." )]
[assembly: AssemblyCompany( "MGR-Infó Bt." )]
[assembly: AssemblyCopyright( "Copyright © MGR-Infó Bt. 2016" )]

[assembly: ComVisible( false ), Guid( "a58f56be-44fb-4cce-b44b-560bbdf73a87" )]
[assembly: CLSCompliant( true )]

[assembly: AssemblyVersion( "1.0.*" )]
#if DEBUG
[assembly: AssemblyInformationalVersion( "1.0.0-alpha" )]
#else
[assembly: AssemblyInformationalVersion( "1.0.0" )]
#endif
