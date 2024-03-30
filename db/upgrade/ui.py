from datetime import datetime


class UI:
    RED = 31
    BLACK = 30
    PURPLE = 35

    def print_red(text):
        UI.print_color(text, UI.RED)

    def print_black(text):
        UI.print_color(text, UI.BLACK)

    def print_purple(text):
        UI.print_color(text, UI.PURPLE)

    def print_color(text, color):
        print("\033[01;{color}m".format(color=color))
        print()
        print("---------------------------------------------------------------------------------")
        print(f"--- {text} {datetime.now()}")
        print("---------------------------------------------------------------------------------")
        print()
        print("\033[00m")
