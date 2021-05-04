const fs = require('fs')
const cp = require('child_process')
const yargs = require('yargs-parser')
const rimraf = require('rimraf')
const archiver = require('archiver')
const info = require('./Info.json');

(async () => {
    cp.execSync('dotnet "C:\\Program Files\\dotnet\\sdk\\5.0.200\\MSBuild.dll" /p:Configuration=Release')

    rimraf.sync('Release')

    fs.mkdirSync('Release')

    fs.copyFileSync('./bin/Release/Adofaiutils.dll', './Release/AdofaiUtils.dll')

    fs.copyFileSync('./Info.json', './Release/Info.json')

    const args = yargs(process.argv)

    if (args.release) {
        const zip = archiver('zip', {})
        const stream = fs.createWriteStream(require('path').join(__dirname, `AdofaiUtils ${info.Version}.zip`))
        zip.pipe(stream)
        zip.directory('Release/', '.')
        await zip.finalize()
    }
})()
