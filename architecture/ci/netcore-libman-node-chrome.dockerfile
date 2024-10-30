FROM darakeon/netcore-libman-node
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN curl -sL https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb > chrome.deb
RUN apt-get install -y ./chrome.deb
RUN rm ./chrome.deb
ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD=true

RUN echo "echo" >> ~/.bashrc
RUN echo "printf '\e[38;5;46m'" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "echo ------------------------------------ CHROME ------------------------------------" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "printf '\e[38;5;51m'" >> ~/.bashrc
RUN echo "google-chrome --no-sandbox --version" >> ~/.bashrc
RUN echo "printf '\e[38;5;253m'" >> ~/.bashrc
