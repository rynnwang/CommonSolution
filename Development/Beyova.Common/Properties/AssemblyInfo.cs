using System.Reflection;
using System.Runtime.InteropServices;
using System.Resources;
using Beyova.ProgrammingIntelligence;
using System.Runtime.CompilerServices;
using Beyova;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Beyova.Common")]
[assembly: AssemblyConfiguration("")]

[assembly: AssemblyDescription(BeyovaPropertyConstants.AssemblyDescription)]
[assembly: AssemblyCompany(BeyovaPropertyConstants.Company)]
[assembly: AssemblyCopyright(BeyovaPropertyConstants.Copyright)]
[assembly: AssemblyTrademark(BeyovaPropertyConstants.AssemblyTrademark)]

[assembly: AssemblyProduct("Beyova.Common")]
[assembly: AssemblyCulture("")]
[assembly: BeyovaComponent("Beyova.Common", "3.13.5")]

[assembly: InternalsVisibleTo("Beyova.WebExtension, PublicKey=002400000480000094000000060200000024000052534131000400000100010043cc5930e5bede"
+ "45da5820d2c00fe5afd1018228c56bf2a5363be550010faa493fe913e64c9ec1ee68e069ae3a0d"
+ "cf188c8b65572edb15e860d53f393bc44292b3218e760ed1300c3a954dcc5140c81d54ab047c17"
+ "f13169bacb5cd5590606e87ceb662d37ad3cac4675dbaab5b0462b53138e758aa16d685b90ff01"
+ "919e7cc8")]

[assembly: InternalsVisibleTo("Beyova.UnitTestKit, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a77bdcffcc1f21"
+ "9efe5232f329c5db2158316252fb8ae4b64cf9396c6e284e315704a42dfab582d9efe1907d1ebf"
+ "6d5500e680aaf480c6e0ec864fa41d80e4bf1f390d5fae8da8774651cdc99ed79f82362ce27d4c"
+ "5946aee724910bf5eab2177e653d10fc7381dc81f2e0b34292d852aa843c1032385df7f6c70539"
+ "6971cdc4")]

[assembly: InternalsVisibleTo("Beyova.Gravity, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5a7b57833610e"
+ "eab205d44273760e6ca067f3e4526cf11ec5b568dd47df2a4ba82cb0fc4d8a8b9fc0c931a28d93"
+ "6a840be60562353d653aaca3d4482afb9fdc0070830a4ccec1e32af364ba1b3db3b14d66984b03"
+ "daaefd551229b93bc87abcd6552e63f714109c6f71be906d27b0796510f6ac63b43fef03b6e67e"
+ "0318e988")]

[assembly: InternalsVisibleTo("Beyova.CentralAuthentication.SDK, PublicKey=00240000048000009400000006020000002400005253413100040000010001008be4b94ca8e465"
+ "1d750b4763f58c8819bcc46eef3a919aee556be56976a3fddec89073e07280b7ca9370423fcf4d"
+ "59005e8dfb6992be69f845018bf94b1912e943b2f725a12fb7c2b65aa75407668b66e22338ba18"
+ "ceba260cb40d2a25acaa068b82e86bc542b435268a2526fb312897d29c6183b0e5497cfed66472"
+ "1e567adf")]

[assembly: InternalsVisibleTo("Beyova.InternalUtil, PublicKey=00240000048000009400000006020000002400005253413100040000010001001fc9082da98e97"
+ "dfee21c3688a6c84afdd3397b1325dafd860f4ba7c6cab28d12e5ace21698ef4ad6614ae228734"
+ "5e4b0e454aca2edf72412ac0c6b5cfb297f228e2e4ccde74ba12887f115d98c678047190051692"
+ "3e01eaf6b185f85647cb722af049c3e969ef276b07efef3e12ac973808c4b763f5a77e57f62780"
+ "bdbddfe8")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("dedf14b5-6a94-4a49-a900-3dad35a3df6c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: NeutralResourcesLanguageAttribute("en")]
