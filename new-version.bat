hg ci -m "New Version"
hg tag %1
hg up default
hg merge develop
hg ci -m "Publish"
hg up develop
