//
//  WxDelegate.m
//  WechatIOSProject
//
//  Created by BanMing on 2018/7/4.
//  Copyright © 2018年 BanMing. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "WxDelegate.h"

@implementation WxDelegate

#pragma mark - LifeCycle
+(instancetype)sharedManager {
    static dispatch_once_t onceToken;
    static WxDelegate *instance;
    dispatch_once(&onceToken, ^{
        instance = [[WxDelegate alloc] init];
    });
    return instance;
}

#pragma mark - WXApiDelegate
- (void)onResp:(BaseResp *)resp {
    //登陆
    if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *authResp = (SendAuthResp *)resp;
        NSLog(@"%s", &"authResp.errCode:"[authResp.errCode]);
        if(authResp.errCode==0){
                UnitySendMessage("WeChatManager","LogInCallBack",[authResp.code UTF8String]);
        }else{
            UnitySendMessage("WeChatManager","LogInCallBack","error");
        }
    }else if ([resp isKindOfClass:[SendMessageToWXResp class]]){
        SendMessageToWXResp *msgResp=(SendMessageToWXResp *)resp;
        NSLog(@"%s", &"msgResp.errCode:"[msgResp.errCode]);
        if (msgResp.errCode==0) {
            //unity分享成功
            UnitySendMessage("WeChatManager","ShareCallBack","ok");
        }else{
            //分享失败
            UnitySendMessage("WeChatManager","ShareCallBack","error");
        }
    }else if ([resp isKindOfClass:[PayResp class]]){
        
        PayResp *payResp=(PayResp * ) resp;
        NSLog(@"%s", &"payResp.errCode:"[payResp.errCode]);
        if (payResp.errCode==0) {
            //支付成功
            UnitySendMessage("WeChatManager","WxPayCallBack","ok");
        }else{
            //支付失败
            UnitySendMessage("WeChatManager","WxPayCallBack","error");
        }
    }
    
}

- (void)onReq:(BaseReq *)req {}

@end
