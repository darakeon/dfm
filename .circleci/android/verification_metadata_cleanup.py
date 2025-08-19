from json import loads
from sys import argv
from re import search
from os import environ, path
from xml.etree import ElementTree


file_path = path.join(
	path.dirname(__file__),
	"..", "..", "android", "gradle", "verification-metadata.xml"
)

with open(file_path, "r") as file:
	first_line = file.readline().strip()


tree = ElementTree.parse(file_path)
root = tree.getroot()

tag_format = r"({.+}).+"
tag_prefix = search(tag_format, root.tag).group(1)

namespace = tag_prefix.replace("{", "").replace("}", "")
ElementTree.register_namespace("", namespace)

components = root.find(f"{tag_prefix}components")

libs = {}

for component in components:
	group = component.attrib.get("group")
	name = component.attrib.get("name")
	version = component.attrib.get("version")

	full_name = f"{group}:{name}"

	if full_name not in libs or version > libs[full_name]:
		libs[full_name] = version

c = 0

print("Before:", len(components))

while c < len(components):
	component = components[c]

	group = component.attrib.get("group")
	name = component.attrib.get("name")
	version = component.attrib.get("version")

	full_name = f"{group}:{name}"

	if libs[full_name] != version:
		components.remove(component)
	else:
		c += 1

print("After:", len(components))


tree.write(file_path, encoding="UTF-8", xml_declaration=True)


with open(file_path, "r") as file:
	content = file.readlines()

content[0] = content[0].replace("'", '"')
content = [line.replace(" />", "/>") for line in content]
content[-1] += "\n"

with open(file_path, "w") as file:
	file.writelines(content)
