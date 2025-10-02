import requests

def get_latest_version(package_name: str) -> str:
	url = f"https://pypi.org/pypi/{package_name}/json"
	response = requests.get(url, timeout=10)
	if response.status_code == 200:
		data = response.json()
		return data["info"]["version"]
	else:
		print(f"[WARN] Could not find {package_name} at PyPI")
		return None


def update_requirements(file_path: str = "requirements.txt"):
	updated_lines = []

	with open(file_path, "r", encoding="utf-8") as file:
		for line in file:
			line = line.strip()
			if not line or line.startswith("#"):
				updated_lines.append(line)
				continue

			if "==" in line:
				package, _ = line.split("==", 1)
			else:
				package = line

			latest_version = get_latest_version(package)
			if latest_version:
				updated_line = f"{package}=={latest_version}"
				print(f"Updated: {line} -> {updated_line}")
				updated_lines.append(updated_line)
			else:
				updated_lines.append(line)

	with open(file_path, "w", encoding="utf-8") as file:
		file.write("\n".join(updated_lines) + "\n")

if __name__ == "__main__":
	update_requirements("src/requirements.txt")
