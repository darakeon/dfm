<img src="site/MVC/Assets/images/pig.svg" height="85" align="right"/>

# COLLABORATE

If you want to help at Don't fly Money - or, lets be more intimate,
DfM -, you are welcome!

Lets just make somethings clearer, to not make it a huge mess...

## Branches

I usually develop in a branch with the version number. When I finish
the version, I open a [pull request](../../pulls) to branch `main`. The name
of the branch you will use develop, you can choose what better fits
you.

## Versioning

The document [RELEASES.md](docs/RELEASES.md) has what was done in each version.
Remember to add your new version, numbering it according to the
description at the beginning of the document. Add it below the version
in progress (the last one, where all the tasks are unchecked).

Then, there is a folder called `version` at project root. Inside of it,
there is a project in Rust to change places where the version number
is. You can:

- use `cargo run`: better one!
- do it manually: it will be boring and you can forget something;
- ask us to change the version and make the commit: good one too!

## GitCop

There is a bot hearing your PRs. Its name is GitCop, and it will look
all the commits and tell if it follows some rules. Yeah, I know, the
history has almost 1400 commits that definitely do not do that, but
lets just watch our steps from now on, ok?

### Commit message format

The message need to have one subject and may have a body. The subject
should summarize what is done, must have no more than 50 characters,
start with the scope - part of the system that is been changed -,
followed by `:`, then by the description of what is done, what should
start with a infinitive verb.

The allowed types are:
- `safe`: core > user;
- `admin`: core > accounts and categories;
- `money`: core > moves;
- `report`: core > reports;
- `robot`: core > schedules;
- `site`: ui, mvc;
- `android`: app;
- `chore`: little changes, refactors;
- `version`: only used to change version numbers;
- `docs`: documentation at the repository;
- `tests`: automated tests;
- `tools`: update version of stuff that is being used;
- `security`: things like error reporting;
- `db`: database changes;
- `ci`: deploy automation;
- `mail`: e-mail sending;
- `language`: translations;
- `publish`: automated publishing pieces;
- `accessibility`: changes to make interface more accessible to everyone
- `legal`: changes about laws

The body of the message is the part where the details go, if they are
necessary. To make readability easier, it must have no more than 72
characters at each line and should separate from subject and between
paragraphs by one blank line.

And, please, explain what you have done the simplest way you can.
Coding is our form of art, but the beautiful on it is other people 
understanding what you did. This rule applies to your code too.

## Don't know what to do?

It's probably not the case, but... There is a [to-do list](docs/TODO.md).
You can pickup a task from any place of the list, the order set to this
file is the order @darakeon intend to do them. Once it's ready, the new
system version number will depend the column type and the current
version.
