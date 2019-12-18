FROM microsoft/aspnet

ADD scripts/installChoco.ps1 /installChoco.ps1
RUN powershell .\installChoco.ps1 -Wait; Remove-Item c:\installChoco.ps1 -Force;

RUN choco install mongodb

WORKDIR /app
COPY .LocationApi/bin .

ENTRYPOINT ["dotnet", "LocationApi.dll"]