FROM darakeon/netcore-libman-node
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN curl -sL https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb > chrome.deb
RUN apt-get install -y ./chrome.deb
RUN rm ./chrome.deb
ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true
