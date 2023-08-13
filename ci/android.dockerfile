FROM darakeon/ubuntu
LABEL maintainer="Dara Keon <laboon@darakeon.com>"
RUN maintain

RUN apt-get install -y openjdk-17-jdk

RUN curl -sL https://services.gradle.org/distributions/gradle-8.2.1-all.zip > /tmp/gradle.zip \
	&& unzip -d /opt/gradle /tmp/gradle.zip \
	&& rm /tmp/gradle.zip
ENV GRADLE_HOME=/opt/gradle/gradle-8.2.1
ENV PATH=${GRADLE_HOME}/bin/:${PATH}
ENV GRADLE_USER_HOME=/var/cache/gradle

ENV ANDROID_SDK_ROOT=/usr/lib/android-sdk
ENV PATH=${ANDROID_SDK_ROOT}/cmdline-tools/bin/:${PATH}
RUN apt-get install -y android-sdk \
	&& curl -sL https://dl.google.com/android/repository/commandlinetools-linux-6858069_latest.zip > /tmp/sdkmanager.zip \
	&& unzip -d $ANDROID_SDK_ROOT /tmp/sdkmanager.zip \
	&& rm /tmp/sdkmanager.zip \
	&& yes | sdkmanager --licenses --sdk_root=$ANDROID_SDK_ROOT

RUN gradle -v # to avoid excessive info at gradle version check

RUN echo "echo" >> ~/.bashrc
RUN echo "printf '\e[38;5;46m'" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "echo ------------------------------------- JAVA -------------------------------------" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "printf '\e[38;5;51m'" >> ~/.bashrc
RUN echo "java -version" >> ~/.bashrc
RUN echo "printf '\e[38;5;46m'" >> ~/.bashrc
RUN echo "echo" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "echo ------------------------------------ GRADLE ------------------------------------" >> ~/.bashrc
RUN echo "echo --------------------------------------------------------------------------------" >> ~/.bashrc
RUN echo "printf '\e[38;5;51m'" >> ~/.bashrc
RUN echo "gradle -v" >> ~/.bashrc
RUN echo "printf '\e[38;5;253m'" >> ~/.bashrc

CMD bash
