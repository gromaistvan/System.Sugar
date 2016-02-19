using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Sugar.Version;

[assembly: AssemblyProduct( "System.Sugar" )]
[assembly: AssemblyTitle( "System.Sugar" )]
[assembly: AssemblyDescription( "Useful .NET extensions." )]
[assembly: AssemblyCompany( "MGR-Infó Bt." )]
[assembly: AssemblyCopyright( "Copyright © MGR-Infó Bt. 2016" )]

[assembly: ComVisible( false ), Guid( "a58f56be-44fb-4cce-b44b-560bbdf73a87" )]
[assembly: CLSCompliant( true )]

[assembly: AssemblyVersion( Major + "." + Minor + ".*" )]
[assembly: AssemblyFileVersion( Major + "." + Minor + "." + Patch + ".0" )]
#if DEBUG
[assembly: AssemblyInformationalVersion( Major + "." + Minor + "." + Patch + "-alpha" )]
#else
[assembly: AssemblyInformationalVersion( Major + "." + Minor + "." + Patch )]
#endif

[assembly: InternalsVisibleTo( "System.Sugar.Quality, PublicKey=" +
    "0024000004800000940000000602000000240000525341310004000001000100" +
    "6F6884DF1534E3FA1039CCBF4763AC67F8027299AAE5C091EE058F53D0E44A49" +
    "CCFD1B44A5E6C0FDB1EE1568DB5D83E24219CBD6EEDFE400E126B65AD0678965" +
    "6F65ABC5E24419BC0BEB390EF11D2B69AEC29BFE263FC1D80AC335EE677A215A" +
    "8A92225A0F8BB4378BD05EE3305DBC8F4AD0FF03A4E9A7062B87645F32FE9792" )]
