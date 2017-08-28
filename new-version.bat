if "%1" == "" (
	echo "no tag"
) else (
	hg ci -m "New Version"
	hg tag %1
	hg up default
	hg merge develop
	hg ci -m "Publish"
	hg up develop
	hg push bitbucket
)