FROM darakeon/netcore-libman
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs

RUN echo "echo" >> ~/.bashrc
RUN echo "printf '\e[38;5;46m'" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "echo ------------------------------------- NODE -------------------------------------" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "printf '\e[38;5;51m'" >> ~/.bashrc
RUN echo "node --version" >> ~/.bashrc
RUN echo "printf '\e[38;5;253m'" >> ~/.bashrc
