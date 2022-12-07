{-# LANGUAGE DeriveFoldable #-}

module Main where

import Data.Char (isDigit)
import Data.List (isPrefixOf)
import System.Environment (lookupEnv)

main :: IO ()
main = do
  input <- lines <$> readFile "input.txt"
  let filesystem = parseFilesystem input
  part <- lookupEnv "part"
  let partFun = if part == Just "part2" then part2 else part1
  print $ partFun filesystem

part1 :: Item Integer -> Integer
part1 = sum . filter (<= 100000) . map sum . filter isDir . list

part2 :: Item Integer -> Integer
part2 fs = minimum $ filter (>= missing) $ map sum $ filter isDir $ list fs
  where
    missing = 30000000 - (70000000 - sum fs)

data Item a = File a | Dir [Item a]
  deriving (Foldable)

isDir :: Item a -> Bool
isDir (File _) = False
isDir (Dir _)  = True

list :: Item a -> [Item a]
list = visit (:) []

visit :: (Item a -> b -> b) -> b -> Item a -> b
visit f b (File a) = f (File a) b
visit f b (Dir is) = f (Dir is) (foldl (visit f) b is)

parseFilesystem :: [String] -> Item Integer
parseFilesystem lines = Dir $ foldl1 (\a -> (Dir a:)) $ foldl collect [] commands
  where
    commands = groupCommands lines
    collect (dir:cur:dirs) ["$ cd .."] = (Dir dir:cur):dirs
    collect dirs           ("$ ls":is) = readFiles is:dirs
    collect dirs           _           = dirs
    readFiles = map (File . read . takeWhile isDigit) . filter (not . isPrefixOf "dir")

groupCommands :: [String] -> [[String]]
groupCommands [] = []
groupCommands lines = command : groupCommands rest
  where
    (command, rest) = takeCommand lines

takeCommand :: [String] -> ([String], [String])
takeCommand (command:lines) = (command:output, rest)
  where
    (output, rest) = break (isPrefixOf "$") lines
