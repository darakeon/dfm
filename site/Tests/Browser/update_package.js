import fs from "fs";

async function getLatestVersion(pkg) {
	const url = `https://registry.npmjs.org/${pkg}/latest`;
	const res = await fetch(url);
	if (!res.ok) {
		console.warn(`[WARN] Could not get version of ${pkg} at npm`);
		return null;
	}
	const data = await res.json();
	return data.version;
}

async function updateDeps(deps) {
	if (!deps) return {};
	const updated = {};
	for (const [pkg, _version] of Object.entries(deps)) {
		const latest = await getLatestVersion(pkg);
		if (latest) {
			console.log(`Updated: ${pkg} -> ^${latest}`);
			updated[pkg] = `^${latest}`;
		} else {
			updated[pkg] = _version;
		}
	}
	return updated;
}

async function updatePackageJson(filePath = "package.json") {
	const pkgData = JSON.parse(fs.readFileSync(filePath, "utf-8"));

	pkgData.dependencies = await updateDeps(pkgData.dependencies);
	pkgData.devDependencies = await updateDeps(pkgData.devDependencies);

	fs.writeFileSync(filePath, JSON.stringify(pkgData, null, '\t') + "\n", "utf-8");
	console.log(`\n✅ ${filePath} successfully updated!`);
}

updatePackageJson();
