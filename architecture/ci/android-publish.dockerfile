FROM darakeon/android
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN apt-get install -y python3-pip
RUN pip install google-api-python-client --break-system-packages

RUN echo "echo" >> ~/.bashrc
RUN echo "printf '\e[38;5;46m'" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "echo ------------------------------------- PIP. -------------------------------------" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "printf '\e[38;5;51m'" >> ~/.bashrc
RUN echo "pip --version" >> ~/.bashrc

CMD bash
