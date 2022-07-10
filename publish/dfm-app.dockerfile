FROM darakeon/ubuntu

RUN apt-get install -y default-jdk
ENV JAVA_HOME=/usr/lib/jvm/java-11-openjdk-amd64

RUN curl -sL https://services.gradle.org/distributions/gradle-7.1.1-all.zip > /tmp/gradle.zip \
	&& unzip -d /opt/gradle /tmp/gradle.zip \
	&& rm /tmp/gradle.zip
ENV GRADLE_HOME=/opt/gradle/gradle-7.1.1
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

RUN mkdir /var/build
WORKDIR /var/build
