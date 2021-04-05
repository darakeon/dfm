cat .circleci/pieces/*.yml > .circleci/config.yml

if [ "$1" != "check" ]; then
	git add .circleci/config.yml
fi
