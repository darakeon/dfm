FROM ubuntu:20.04
MAINTAINER Dara Keon
RUN apt upgrade -y && apt update && apt autoremove -y

RUN apt install -y curl
RUN apt install -y ca-certificates
RUN curl https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb > packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt update
RUN apt install -y apt-transport-https
RUN apt install -y dotnet-sdk-5.0
RUN apt install -y aspnetcore-runtime-5.0

RUN echo 'export PS1="\n\n[\A] \u@\W\$ "' >> ~/.bashrc
