
-- ///////////////// New Default Table Footbal user /////////////////
INSERT INTO ChannelUser (NameId,Name,NameUrlSafe,IsAccessible,IsVisible,Bearer)
VALUES ('06F4D665-AEBF-4704-868F-D052D097C1F5','Table football','tafelvoetbal','1','1','kHZ6ody2nQ9dmcMSCk5m');

-- // add test user // --
INSERT INTO ChannelUser (NameId,Name,NameUrlSafe,IsAccessible,IsVisible,Bearer)
VALUES ('cc9299c5-03a6-409a-be35-30981acfa7ac','Test Channel','test','1','0','kHZ6ody2nQ9dmcMSCk5m');

-- // example Event // --
INSERT INTO ChannelEvent (DateTime,ChannelUserId,Status,Weight)
VALUES ('2018-01-10 21:42:01.4988005','06F4D665-AEBF-4704-868F-D052D097C1F5',1,3);

