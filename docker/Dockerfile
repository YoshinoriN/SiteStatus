FROM mcr.microsoft.com/dotnet/runtime:5.0

COPY ./dist/ /usr/opt/SiteStatus
WORKDIR /usr/opt/SiteStatus
COPY ./entry-point.sh /usr/opt/SiteStatus

ENTRYPOINT [ "/usr/opt/SiteStatus/entry-point.sh" ]