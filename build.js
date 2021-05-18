const fs = require('fs')
const cp = require('child_process')
const yargs = require('yargs-parser')
const rimraf = require('rimraf')
const archiver = require('archiver')
const info = require('./Info.json')
const path = require('path')
const { findSteamAppById } = require('find-steam-app');

(async () => {
    rimraf.sync('Release')
    
    cp.execSync('"F:\\Programs\\JetBrains Rider 2021.1.2\\tools\\MSBuild\\Current\\Bin\\MSBuild.exe" /p:Configuration=Release')

    fs.mkdirSync('Release')

    fs.copyFileSync(`./bin/Release/${info.Id}.dll`, `./Release/${info.Id}.dll`)

    fs.copyFileSync('./Info.json', './Release/Info.json')

    const args = yargs(process.argv)

    if (args.release) {
        const zip = archiver('zip', {})
        const stream = fs.createWriteStream(path.join(__dirname, `${info.Id}-${info.Version}.zip`))
        zip.pipe(stream)
        zip.directory('Release/', info.Id)
        await zip.finalize()
    } else {
        const appPath = await findSteamAppById(977950)
        const modPath = path.join(appPath, 'Mods', info.Id)
        const r68ModPath = path.join('F:\\Programs\\Steam\\steamapps\\common\\R68 Dance of Fire and Ice', 'Mods', info.Id)
        rimraf.sync(path.join(modPath, info.Id + '.dll'))
        rimraf.sync(path.join(modPath, 'Info.json'))
        rimraf.sync(path.join(r68ModPath, info.Id + '.dll'))
        rimraf.sync(path.join(r68ModPath, 'Info.json'))
        
        if (!fs.existsSync(modPath)) fs.mkdirSync(modPath)
        if (!fs.existsSync(r68ModPath)) fs.mkdirSync(r68ModPath)
        fs.copyFileSync(`Release/${info.Id}.dll`, path.join(modPath, info.Id + '.dll'))
        fs.copyFileSync('Release/Info.json', path.join(modPath, 'Info.json'))
        fs.copyFileSync(`Release/${info.Id}.dll`, path.join(r68ModPath, info.Id + '.dll'))
        fs.copyFileSync('Release/Info.json', path.join(r68ModPath, 'Info.json'))
    }
    console.log('Successful.')
})()
