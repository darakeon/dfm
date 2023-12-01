MAKEFLAGS += --no-print-directory
BRANCH:=$$(cat branch.git)

help:
	@echo
	@echo "    |-----------------------------------------------------------------------------|"
	@echo "    |                                                                             |"
	@echo "    |  Need parameters to get tasks from TODO and create new version at RELEASES  |"
	@echo "    |                                                                             |"
	@echo "    |    make translate                                                           |"
	@echo "    |    >>> open task translations file;                                         |"
	@echo "    |                                                                             |"
	@echo "    |    make new_by_quantity QUANTITY={N}                                        |"
	@echo "    |    >>> get first N tasks;                                                   |"
	@echo "    |                                                                             |"
	@echo "    |    make new_by_numbers NUMBERS=\"{p1} {p2} {p3} ...\"                         |"
	@echo "    |    >>> get tasks by position;                                               |"
	@echo "    |                                                                             |"
	@echo "    |    make existing                                                            |"
	@echo "    |    >>> to use when the new release is already created                       |"
	@echo "    |                                                                             |"
	@echo "    |    make check                                                               |"
	@echo "    |    >>> just check if the version is right.                                  |"
	@echo "    |                                                                             |"
	@echo "    |    make create_old_tag_and_new_branch                                       |"
	@echo "    |    >>> start new version at git tree (branch, tag, clear remote)            |"
	@echo "    |                                                                             |"
	@echo "    |                             use just numbers at parameters, not the braces  |"
	@echo "    |                                                                             |"
	@echo "    |-----------------------------------------------------------------------------|"

translate:
	@code ../docs/translations.json

new_by_quantity:
	@cargo run -q -- -q ${QUANTITY}

new_by_numbers:
	@cargo run -q -- -n ${NUMBERS}

existing:
	@cargo run -q -- -e

check:
	@cargo run -q -- -c

create_old_tag_and_new_branch:
	@git branch --show-current > branch.git
	@git checkout main
	@git branch -d ${BRANCH}
	@git pull --prune
	@git tag -a ${BRANCH}
	@git push --tags
	@head -n 8 ../docs/RELEASES.md | tail -n 1 | cut -d "#" -f 2 | cut -d ")" -f 1 > branch.git
	@git checkout -b ${BRANCH}
	@git push -u origin ${BRANCH}
	@git branch -d main
	@rm branch.git

clear:
	@git branch --show-current > branch.git
	@git checkout main
	@git branch -d ${BRANCH}
	@git pull --prune
	@head -n 8 ../docs/RELEASES.md | tail -n 1 | cut -d "#" -f 2 | cut -d ")" -f 1 > branch.git
	@git checkout ${BRANCH}
	@git branch -d main
	@rm branch.git
