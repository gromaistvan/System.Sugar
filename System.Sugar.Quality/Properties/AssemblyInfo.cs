using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Properties.Version;

[assembly: AssemblyProduct( "System.Sugar" )]
[assembly: AssemblyTitle( "System.Sugar.Quality" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyCompany( "MGR-Infó Bt." )]
[assembly: AssemblyCopyright( "Copyright © MGR-Infó Bt. 2016" )]

[assembly: ComVisible( false ), Guid( "0934a6ce-3b89-41f9-a436-1b8fcf6656b9" )]
[assembly: CLSCompliant( true )]

[assembly: AssemblyVersion( Major + "." + Minor + ".*" )]
[assembly: AssemblyFileVersion( Major + "." + Minor + "." + Patch + ".0" )]
#if DEBUG
[assembly: AssemblyInformationalVersion( Major + "." + Minor + "." + Patch + "-alpha" )]
#else
[assembly: AssemblyInformationalVersion( Major + "." + Minor + "." + Patch )]
#endif