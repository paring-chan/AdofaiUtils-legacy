// 정현수 is babo
// 패트릭 is gosu

const parse = require('semver/functions/parse')
const yargs = require('yargs-parser')
const Info = require('./Info.json')
const fs = require('fs')
const cp = require('child_process')

const args = yargs(process.argv)

const level = args.l || 'patch'

if (!['minor', 'major', 'patch'].includes(level)) {
    return process.exit(1)
}

const ver = parse(Info.Version)

ver[level]++

if (level === 'minor') {
    ver.patch = 0
}

if (level === 'major') {
    ver.patch = 0
    ver.minor = 0
}

const finalVer = [ver.major,ver.minor,ver.patch].join('.')

Info.Version = finalVer

fs.writeFileSync('Info.json', JSON.stringify(Info))

fs.writeFileSync('Repository.json', JSON.stringify({
    Releases: [
        {
            Id: 'AdofaiUtils',
            Version: Info.Version,
            DownloadUrl: `https://github.com/pikokr/AdofaiUtils/download/v${Info.Version}/${Info.Id}-${Info.Version}.zip`
        }
    ]
}))

cp.execSync('git add .')
cp.execSync(`git commit -m v${Info.Version}`)
cp.execSync(`git tag v${Info.Version}`)

console.log('Successful.')
