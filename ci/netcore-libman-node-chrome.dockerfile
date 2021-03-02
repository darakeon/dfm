FROM darakeon/netcore-libman-node
MAINTAINER Dara Keon
RUN apt update

RUN curl https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb > google-chrome-stable_current_amd64.deb
RUN apt install -y ./google-chrome-stable_current_amd64.deb
ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true
