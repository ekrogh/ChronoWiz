dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.*
dotnet publish ChronoWiz/ChronoWiz.csproj -f net9.0-ios -c Release -p:ArchiveOnBuild=true -p:RuntimeIdentifier=ios-arm64 -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz_Ad_Hoc_Distribution_Profile" -o "/Users/eks/Downloads/"