/*
 Navicat Premium Data Transfer

 Source Server         : LOCAL
 Source Server Type    : MySQL
 Source Server Version : 80025
 Source Host           : localhost:3306
 Source Schema         : yd

 Target Server Type    : MySQL
 Target Server Version : 80025
 File Encoding         : 65001

 Date: 22/06/2022 10:32:25
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for upvermessage
-- ----------------------------
DROP TABLE IF EXISTS `upvermessage`;
CREATE TABLE `upvermessage`  (
  `ID` bigint NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `CurrentVer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '当前版本号',
  `UpVer` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '更新版本号',
  `DeviceType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '设备类型',
  `Ip` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT 'IP地址',
  `Port` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '端口',
  `UpFlg` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '更新状态',
  `UpDate` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT '日期',
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of upvermessage
-- ----------------------------
INSERT INTO `upvermessage` VALUES (1, '1.0.1', '1.0.2', 'VIDEO-STANDARD', 'None', 'None', '版本更新成功', '2021-11-08 20:25:04');
INSERT INTO `upvermessage` VALUES (2, '1.0.2', '1.0.3', 'VIDEO-STANDARD', 'None', 'None', '版本更新成功', '2021-11-08 20:29:17');
INSERT INTO `upvermessage` VALUES (3, '1.0.0', '1.0.2', 'TURNSTILE_PROCESSOR', '192.168.31.106', '22261', '版本更新成功', '2021-11-08 22:41:13');
INSERT INTO `upvermessage` VALUES (4, '1.0.1', '1.0.2', 'ELEVATOR_SECONDARY', '192.168.31.106', '23262', '版本更新成功', '2021-11-08 22:43:36');
INSERT INTO `upvermessage` VALUES (5, '1.0.0', '1.0.3', 'VIDEO-STANDARD', 'None', 'None', '版本更新成功', '2021-11-08 22:56:32');

SET FOREIGN_KEY_CHECKS = 1;
