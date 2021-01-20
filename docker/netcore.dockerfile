FROM ubuntu:20.10
MAINTAINER Dara Keon
RUN apt update

RUN apt install -y curl
RUN curl https://packages.microsoft.com/config/ubuntu/20.10/packages-microsoft-prod.deb > packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt install -y apt-transport-https
RUN apt update
RUN apt install -y dotnet-sdk-5.0
RUN apt install -y aspnetcore-runtime-5.0

RUN echo 'export PS1="\n\n[\A] \u@\W\$ "' >> ~/.bashrc
