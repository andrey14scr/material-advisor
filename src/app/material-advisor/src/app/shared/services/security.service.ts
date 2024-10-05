import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import crypto from 'crypto-js';

@Injectable({
    providedIn: 'root'
})
export class SecurityService {
  private key: string;

  constructor() {
    this.key = environment.hashKey;
  }
  
  getHash(input: string) {
    return crypto.HmacSHA256(input, this.key).toString();
  }
}

