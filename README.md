# ReSharperPlugin.XUnitTestRunnerSpeedBooster

## DISCLAIMER
__If you see any anomalies in test execution after this extension installation, please delete the extension before investigation.__

## Description

ReSharper Plugin to speed up XUnit tests execution.
The improvement in speed is achieved by applying fast cast check to `TestMessageVisitor` that is heavily used by xunit integration. Basically, that is a backport of functionality from `TestMessageSink`.

See applied changes in [this commit](https://github.com/Zvirja/xunit/commit/cb6e65fd18c12125f42d3a0672dffc3229382daa).

Wish has been already [reported to R# issue tracker] (https://youtrack.jetbrains.com/issue/RSRP-460644), but it could take time before this change is finaly released.

