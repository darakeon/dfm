FROM darakeon/netcore
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="/root/.dotnet/tools:${PATH}"

RUN echo "echo" >> ~/.bashrc
RUN echo "printf '\e[38;5;46m'" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "echo ------------------------------------ LIBMAN ------------------------------------" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "printf '\e[38;5;51m'" >> ~/.bashrc
RUN echo "libman --version" >> ~/.bashrc
RUN echo "printf '\e[38;5;253m'" >> ~/.bashrc
