dotnet build -t:Clean
dotnet clean
dotnet publish ChronoWiz/ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz_Mac_App_Store" -o "/Users/eks/Downloads/"
