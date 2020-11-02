import { HubConnectionBuilder } from '@microsoft/signalr';

export function getHubClient(hubUrl) {
  return new HubConnectionBuilder()
    .withUrl(hubUrl)
    .withAutomaticReconnect()
    .build();
}