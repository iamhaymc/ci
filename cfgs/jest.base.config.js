/** @type {import('ts-jest').JestConfigWithTsJest} */
module.exports = {
  testMatch: ["**/*.(test|spec).ts"],
  testPathIgnorePatterns: ["/node_modules/", "/dist/"],
  moduleFileExtensions: [
    "ts",
    "tsx",
    "mts",
    "js",
    "jsx",
    "mjs",
    "json",
    "node",
  ],
  transform: {
    "^.+\\.m?tsx?$": "ts-jest",
    "^.+\\.m?jsx?$": "babel-jest",
  },
  transformIgnorePatterns: ["node_modules/(?!.)"],
  testEnvironment: "node", // or 'jsdom'
};
