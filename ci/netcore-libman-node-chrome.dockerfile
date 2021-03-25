FROM darakeon/netcore-libman-node
MAINTAINER Dara Keon
RUN maintain

RUN curl -sL https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb > chrome.deb
RUN apt install -y ./chrome.deb
RUN rm ./chrome.deb
ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true
